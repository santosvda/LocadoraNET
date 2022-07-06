
import { Button, Form, Table, Card, Space, message, Skeleton, DatePicker, Select, Alert} from 'antd';
import { DownloadOutlined } from '@ant-design/icons';
import React, { useEffect, useState } from 'react';
import moment from 'moment'

import api from "./services/api";

function Locacao() {
    const [locacoes, setLocacoes] = useState();
    const [clientes, setClientes] = useState();
    const [filmes, setFilmes] = useState();
    const [editable, setEditable] = useState({ editable: false, id: 0 });
    const [loading, setLoading] = useState(true);
    const [lancamento, setLancamento] = useState(false);
    const [form] = Form.useForm()

    const columns = [
        {
            title: 'Código',
            dataIndex: 'id',
            width: '10%',
        },
        {
            title: 'Filme',
            dataIndex: 'titulo',
            width: '30%',
        },
        {
            title: 'Cliente',
            dataIndex: 'nome',
            width: '30%',
        },
        {
            title: 'Data Devolução',
            dataIndex: 'dataDevolucao',
            width: '15%',
        },
        {
            title: 'Data Locação',
            dataIndex: 'dataDevolucao',
            width: '15%',
        },
        {
            title: 'Ações',
            key: 'action',
            render: (_, record) => (
                <Space size="middle">
                    <a type="text" onClick={() => {
                        setEditable(
                            { editable: true, id: record.id }
                        )
                        form.setFieldsValue({
                            clienteId: record.cliente.id,
                            filmeId: record.filme.id,
                            dataDevolucao: moment(record.dataDevolucao, "DD/MM/YYYY"),
                            dataLocacao: moment(record.dataLocacao, "DD/MM/YYYY")
                        })
                    }}>Editar</a>
                    <Button type="text" style={{ color: '#ff4d4f' }} onClick={() => removerLocacao(record.id)}>Remover</Button>
                </Space>
            ),
        },
    ];

    function getLocacoes() {
        setLoading(true)
        cancelarEdicao()
        api
            .get("/locacao")
            .then((response) => {
                setLocacoes(response.data.map(d => ({
                    ...d,
                    key: d.id,
                    nome: d.cliente.nome,
                    titulo: d.filme.titulo,
                    dataDevolucao: moment(d.dataDevolucao, "DD/MM/YYYY").format('DD/MM/YYYY'),
                    dataLocacao: moment(d.dataLocacao, "DD/MM/YYYY").format('DD/MM/YYYY')
                })))
            })
            .catch((err) => {
                message.error('Ocorreu um erro!')
            })
            .finally(() => setLoading(false));
    }
    function getClientes() {
        api
            .get("/cliente")
            .then((response) => {
                setClientes(response.data.map(d => ({
                    label: d.nome,
                    value: d.id,
                })))
            })
            .catch(() => {
                message.error('Ocorreu um erro!')
            })
    }
    function getFilmes() {
        api
            .get("/filme")
            .then((response) => {
                setFilmes(response.data.map(d => ({
                    ...d,
                    label: d.titulo,
                    value: d.id,
                })))
            })
            .catch(() => {
                message.error('Ocorreu um erro!')
            })
    }
    useEffect(() => {
        getLocacoes()
        getClientes()
        getFilmes()
    }, []);
    function onFinish(values) {
        if (!editable.editable) {
            api
                .post("/locacao",
                    {
                        clienteId: values.clienteId
                        , filmeId: values.filmeId
                        , dataDevolucao: moment(values.dataDevolucao).format("DD/MM/YYYY")
                        , dataLocacao: moment(values.dataLocacao).format("DD/MM/YYYY")
                    })
                .then(() => {
                    message.success('Locação cadastrada com sucesso!')
                    getLocacoes()
                })
                .catch((err) => {
                    console.log(err)
                    message.error('Ocorreu um erro!')
                });
        } else {
            api
                .put(`/locacao/${editable.id}`
                    , {
                        clienteId: values.clienteId
                        , filmeId: values.filmeId
                        , dataDevolucao: moment(values.dataDevolucao).format("DD/MM/YYYY")
                        , dataLocacao: moment(values.dataLocacao).format("DD/MM/YYYY")
                    })
                .then(() => {
                    message.success('Locação editada com sucesso!')
                    cancelarEdicao()
                    getLocacoes()
                })
                .catch((err) => {
                    console.log(err)
                    message.error('Ocorreu um erro!')
                });
        }
    };
    function removerLocacao(id) {
        api
            .delete(`locacao/${id}`)
            .then(() => {
                message.success('Locação removida com sucesso!')
                getLocacoes()
            })
            .catch((err) => {
                console.log(err)
                message.error('Ocorreu um erro!')
            });
    }
    function onFinishFailed(errorInfo) {
        console.log('Failed:', errorInfo);
    };
    function cancelarEdicao() {
        setLancamento(false)
        setEditable({ editable: false, id: 0, titulo: '' })
        form.setFieldsValue({
            clienteId: '',
            filmeId: '',
            dataDevolucao: moment(),
            dataLocacao: moment()
        })
    }

    function atualizarDataDevolucao() {
        const { filmeId, dataLocacao } = form.getFieldsValue()
        const filme = filmes.find((f) => f.id === filmeId)
        if (filme.lancamento) {
            form.setFieldsValue({ dataDevolucao: moment(dataLocacao).add(2, 'days') })
            setLancamento(true)
        }
        else {
            form.setFieldsValue({ dataDevolucao: moment(dataLocacao).add(3, 'days') })
            setLancamento(false)
        }
    }


    return (
        <>
            <h1>
                {
                    editable.editable ? `Editando LOCAÇÃO - Cód: ${editable.id}` : 'Cadastro de locacao'
                }
            </h1>
            <Skeleton active loading={loading}>
                <Form
                    form={form}
                    name="basic"
                    labelCol={{ span: 4 }}
                    initialValues={{ clienteId: '', filmeId: '', dataDevolucao: moment(), dataLocacao: moment() }}
                    onFinish={onFinish}
                    onFinishFailed={onFinishFailed}
                    autoComplete="off"
                >
                    <Form.Item
                        label="Cliente"
                        name="clienteId"
                        rules={[
                            { required: true, message: 'O campo é obrigatório' },
                        ]}
                    >
                        <Select options={clientes} />
                    </Form.Item>

                    <Form.Item
                        label="Filme"
                        name="filmeId"
                        rules={[
                            { required: true, message: 'O campo é obrigatório' },
                        ]}
                    >
                        <Select options={filmes} onChange={atualizarDataDevolucao} />
                    </Form.Item>

                    <Form.Item
                        label="Data de Locação"
                        name="dataLocacao"
                        rules={[{ required: true, message: 'O campo é obrigatório' }]}
                    >
                        <DatePicker format="DD/MM/YYYY" onChange={atualizarDataDevolucao} />
                    </Form.Item>

                    <Form.Item
                        label="Data de Devolução"
                        name="dataDevolucao"
                        rules={[{ required: true, message: 'O campo é obrigatório' }]}
                    >
                        <DatePicker disabled format="DD/MM/YYYY" />
                    </Form.Item>
                    <Form.Item wrapperCol={{ offset: 4, span: 16 }}>
                        <Button style={{ marginRight: '8px' }} type="primary" htmlType="submit">
                            {editable.editable ? 'Editar' : 'Salvar'}
                        </Button>
                        {
                            editable.editable ?
                                <Button type="danger" onClick={cancelarEdicao}>
                                    Cancelar
                                </Button> : ''
                        }
                    </Form.Item>
                    {
                        lancamento ?
                            <Alert style={{ marginBottom: '10px' }} message="Obs!" type="info" description="Locação de um filme lançamento! Data de Devolução de 2 Dias." showIcon closable />
                            : null
                    }
                </Form>
                <Card>
                    <Button onClick={getLocacoes} style={{ marginBottom: '10px' }} type="primary" shape="round" icon={<DownloadOutlined />}>
                        Atualizar
                    </Button>
                    <Table scroll columns={columns} dataSource={locacoes} />
                </Card>
            </Skeleton>
        </>


    );
}

export default Locacao;