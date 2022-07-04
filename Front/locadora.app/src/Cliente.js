
  import { Button, DatePicker, Form, Input, Table, Card, Space  } from 'antd';
  import React from 'react';

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
            <a href='#' onClick={() => console.log('Salvar', record)}>Editar</a>
            <a href='#' onClick={() => console.log('Remover', record)}>Remover</a>
          </Space>
        ),
      },
  ];
  const data = [];

for (let i = 0; i < 46; i++) {
  data.push({
    key: i,
    nome: `Edward King ${i}`,
    cpf: '01234567891',
    dataNascimento: `23/02/2000`,
  });
}
  
  function App() {
    function onFinish (values) {
        console.log('Success:', values);
      };
    
      function onFinishFailed (errorInfo) {
        console.log('Failed:', errorInfo);
      };
    return (
        <>
        <Form
            name="basic"
            labelCol={{ span: 4 }}
            initialValues={{ }}
            onFinish={onFinish}
            onFinishFailed={onFinishFailed}
            autoComplete="off"
            >
            <Form.Item
                label="Nome"
                name="nome"
                rules={[{ required: true, message: 'O campo é obrigatório' }]}
            >
                <Input />
            </Form.Item>

            <Form.Item
                label="CPF"
                name="cpf"
                rules={[{ required: true, message: 'O campo é obrigatório'}]}
            >
                <Input maxLength={11}/>
            </Form.Item>

            <Form.Item
                label="Data de Nascimento"
                name="dataNascimento"
                rules={[{ required: true, message: 'O campo é obrigatório' }]}
            >
                <DatePicker format="DD/MM/YYYY"/>
            </Form.Item>

            <Form.Item wrapperCol={{ offset: 4, span: 16 }}>
                <Button type="primary" htmlType="submit">
                Salvar
                </Button>
            </Form.Item>
            </Form>
            <Card>
            <Table scroll columns={columns} dataSource={data} />
            </Card>
            </>


    );
  }
  
  export default App;