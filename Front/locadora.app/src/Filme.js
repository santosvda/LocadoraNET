
import { Button, Form, Input, Table, Card, Space, message, Skeleton, InputNumber, Col, Row, Upload } from 'antd';
import { DownloadOutlined, UploadOutlined } from '@ant-design/icons';
import React, { useEffect, useState } from 'react';

import api from "./services/api";

function Filme() {
    const [filmes, setFilmes] = useState();
    const [editable, setEditable] = useState({ editable: false, id: 0, titulo: '' });
    const [loading, setLoading] = useState(true);
    const [form] = Form.useForm()

    const columns = [
        {
            title: 'Título',
            dataIndex: 'titulo',
            width: '50%',
        },
        {
            title: 'Classificação Indicativa',
            dataIndex: 'classificacaoIndicativa',
            sorter: (a, b) => a.classificacaoIndicativa - b.classificacaoIndicativa,
            width: '25%',
        },
        {
            title: 'Lançamento',
            dataIndex: 'lancamento',
            width: '25%',
            sorter: (a, b) => a.lancamento - b.lancamento,
        },
        {
            title: 'Ações',
            key: 'action',
            render: (_, record) => (
                <Space size="middle">
                    <a href='#' onClick={() => {
                        setEditable(
                            { editable: true, id: record.id, titulo: record.titulo }
                        )
                        form.setFieldsValue({
                            titulo: record.titulo,
                            classificacaoIndicativa: record.classificacaoIndicativa,
                            lancamento: record.lancamento
                        })
                    }}>Editar</a>
                    <a href='#' onClick={() => removerFilme(record.id)}>Remover</a>
                </Space>
            ),
        },
    ];

    function getFilmes() {
        setLoading(true)
        api
            .get("/filme")
            .then((response) => {
                setFilmes(response.data.map(d => ({
                    ...d,
                    key: d.id,
                })))
            })
            .catch((err) => {
                message.error('Ocorreu um erro!')
            })
            .finally(() => setLoading(false));
    }
    useEffect(() => {
        getFilmes()
    }, []);
    function onFinish(values) {
        if (!editable.editable) {
            api
                .post("/filme", { titulo: values.titulo, classificacaoIndicativa: values.classificacaoIndicativa, lancamento: values.lancamento })
                .then((response) => {
                    message.success('Filme cadastrado com sucesso!')
                    getFilmes()
                })
                .catch((err) => {
                    console.log(err)
                    message.error('Ocorreu um erro!')
                });
        } else {
            api
                .put(`/filme/${editable.id}`, { titulo: values.titulo, classificacaoIndicativa: values.classificacaoIndicativa, lancamento: values.lancamento })
                .then((response) => {
                    message.success('Filme editado com sucesso!')
                    cancelarEdicao()
                    getFilmes()
                })
                .catch((err) => {
                    console.log(err)
                    message.error('Ocorreu um erro!')
                });
        }
    };
    function removerFilme(id) {
        api
            .delete(`filme/${id}`)
            .then((response) => {
                message.success('Filme removido com sucesso!')
                getFilmes()
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
        setEditable({ editable: false, id: 0, titulo: '' })
        form.setFieldsValue({
            titulo: '',
            classificacaoIndicativa: '',
            lancamento: ''
        })
    }
    const [fileList, setFileList] = useState([]);

    const props = {
        multi: false,
        accept: '.csv',
        customRequest: (info) => {
            console.log('info', info)
        },

        onChange(info) {
            let fileList = [info.file];
            fileList.forEach(function (file, index) {
                let reader = new FileReader();
                reader.onload = (e) => {
                    file.base64 = e.target.result;
                };
                reader.readAsDataURL(file.originFileObj);
            });
            console.log('fileList', fileList[0])
            if (fileList[0].type !== 'text/csv')
                message.error('Arquivo deve ser do tipo .csv')


            setFileList([])
        },
        fileList
    };

    return (
        <>
            <h1>
                {
                    editable.editable ? `Editando ${editable.titulo}-Cód: ${editable.id}` : 'Cadastro de Filme'
                }
            </h1>
            <Skeleton active loading={loading}>
                <Form
                    form={form}
                    name="basic"
                    labelCol={{ span: 4 }}
                    initialValues={{ titulo: '', classificacaoIndicativa: '', lancamento: '' }}
                    onFinish={onFinish}
                    onFinishFailed={onFinishFailed}
                    autoComplete="off"
                >
                    <Form.Item
                        label="Título"
                        name="titulo"
                        rules={[
                            { required: true, message: 'O campo é obrigatório' },
                            { min: 3, message: 'Título deve conter ao menos 3 caracteres' },
                        ]}
                    >
                        <Input maxLength={100} />
                    </Form.Item>

                    <Form.Item
                        label="Classificação Indicativa"
                        name="classificacaoIndicativa"
                        rules={[
                            { required: true, message: 'O campo é obrigatório' },
                        ]}
                    >
                        <InputNumber max={120} />
                    </Form.Item>

                    <Form.Item
                        label="Lançamento"
                        name="lancamento"
                        rules={[{ required: true, message: 'O campo é obrigatório' }]}
                    >
                        <InputNumber min={1800} max={2500} />
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
                    <Row gutter={0}>
                        <Col span={3}>
                            <Button onClick={getFilmes} style={{ marginBottom: '10px' }} type="primary" shape="round" icon={<DownloadOutlined />}>
                                Atualizar
                            </Button>
                        </Col>
                        <Col span={3}>
                            <Upload {...props}>
                                <Button icon={<UploadOutlined />}>Importar Filmes</Button>
                            </Upload>
                        </Col>
                    </Row>
                    <Table scroll columns={columns} dataSource={filmes} />
                </Card>
            </Skeleton>
        </>


    );
}

export default Filme;