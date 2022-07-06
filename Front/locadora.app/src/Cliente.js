
import { Button, DatePicker, Form, Input, Table, Card, Space, message, Skeleton } from 'antd';
import { DownloadOutlined } from '@ant-design/icons';
import React, { useEffect, useState } from 'react';
import moment from 'moment'

import api from "./services/api";

function Cliente() {
    const [clientes, setClientes] = useState();
    const [editable, setEditable] = useState({ editable: false, id: 0, nome: '' });
    const [loading, setLoading] = useState(true);
    const [form] = Form.useForm()

    const columns = [
        {
            title: 'Nome',
            dataIndex: 'nome',
            width: '30%',
        },
        {
            title: 'CPF',
            dataIndex: 'cpf',
            sorter: (a, b) => a.cpf - b.cpf,
        },
        {
            title: 'Data de Nascimento',
            dataIndex: 'dataNascimento',
            width: '40%',
        },
        {
            title: 'Ações',
            key: 'action',
            render: (_, record) => (
                <Space size="middle">
                    <a href='#' onClick={() => {
                        setEditable(
                            { editable: true, id: record.id, nome: record.nome }
                        )
                        form.setFieldsValue({
                            nome: record.nome,
                            cpf: record.cpf,
                            dataNascimento: moment(record.dataNascimento, "DD/MM/YYYY")
                        })
                    }}>Editar</a>
                    <Button type="text" style={{ color: '#ff4d4f' }} onClick={() => removerCliente(record.id)}>Remover</Button>
                </Space>
            ),
        },
    ];

    function getClientes() {
        setLoading(true)
        api
            .get("/cliente")
            .then((response) => {
                setClientes(response.data.map(d => ({
                    ...d,
                    key: d.id,
                    dataNascimento: moment(d.dataNascimento, "DD/MM/YYYY").format('DD/MM/YYYY')
                })))
            })
            .catch((err) => {
                message.error('Ocorreu um erro!')
            })
            .finally(() => setLoading(false));
    }
    useEffect(() => {
        getClientes()
    }, []);
    function onFinish(values) {
        if (!editable.editable) {
            api
                .post("/cliente", { nome: values.nome, cpf: values.cpf, dataNascimento: moment(values.dataNascimento).format("DD/MM/YYYY") })
                .then((response) => {
                    message.success('Cliente cadastrado com sucesso!')
                    cancelarEdicao()
                    getClientes()
                })
                .catch((err) => {
                    console.log(err)
                    message.error('Ocorreu um erro!')
                });
        } else {
            api
                .put(`/cliente/${editable.id}`, { nome: values.nome, cpf: values.cpf, dataNascimento: moment(values.dataNascimento).format("DD/MM/YYYY") })
                .then((response) => {
                    message.success('Cliente editado com sucesso!')
                    getClientes()
                })
                .catch((err) => {
                    console.log(err)
                    message.error('Ocorreu um erro!')
                });
        }
    };
    function removerCliente(id){
        api
            .delete(`/cliente/${id}`)
            .then((response) => {
                message.success('Cliente removido com sucesso!')
                getClientes()
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
        setEditable({ editable: false, id: 0, nome: '' })
        form.setFieldsValue({
            nome: '',
            cpf: '',
            dataNascimento: moment()
        })
    }


    return (
        <>
            <h1>
                {
                    editable.editable ? `Editando ${editable.nome}-Cód: ${editable.id}` : 'Cadastro de Cliente'
                }
            </h1>
            <Skeleton active loading={loading}>
                <Form
                    form={form}
                    name="basic"
                    labelCol={{ span: 4 }}
                    initialValues={{ nome: '', cpf: '', dataNascimento: moment() }}
                    onFinish={onFinish}
                    onFinishFailed={onFinishFailed}
                    autoComplete="off"
                >
                    <Form.Item
                        label="Nome"
                        name="nome"
                        rules={[
                            { required: true, message: 'O campo é obrigatório' },
                            { min: 3, message: 'Nome deve conter ao menos 3 caracteres' },
                        ]}
                    >
                        <Input maxLength={200} />
                    </Form.Item>

                    <Form.Item
                        label="CPF"
                        name="cpf"
                        rules={[
                            { required: true, message: 'O campo é obrigatório' },
                            { min: 11, message: 'CPF deve conter 11 caracteres' },
                        ]}
                    >
                        <Input maxLength={11} />
                    </Form.Item>

                    <Form.Item
                        label="Data de Nascimento"
                        name="dataNascimento"
                        rules={[{ required: true, message: 'O campo é obrigatório' }]}
                    >
                        <DatePicker format="DD/MM/YYYY" />
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
                </Form>
                <Card>
                    <Button onClick={getClientes} style={{ marginBottom: '10px' }} type="primary" shape="round" icon={<DownloadOutlined />}>
                        Atualizar
                    </Button>
                    <Table scroll columns={columns} dataSource={clientes} />
                </Card>
            </Skeleton>
        </>


    );
}

export default Cliente;