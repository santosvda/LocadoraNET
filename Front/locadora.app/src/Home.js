
import { Button, message } from 'antd';
import { DownloadOutlined } from '@ant-design/icons';
import React from 'react';

import api from "./services/api";

function Home() {

    function getRelatorio() {
        api
            .get("/locacao/gerar/planilha",{
                responseType: 'blob', // important
            })
            .then((response) => {
                const url = window.URL.createObjectURL(new Blob([response.data]));
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', `${Date.now()}.xlsx`);
                document.body.appendChild(link);
                link.click();
                message.success('Arquivo gerado com sucesso!')
            })
            .catch((err) => {
                message.error('Ocorreu um erro!')
            })
    }

    return (
        <>
            <Button onClick={getRelatorio} style={{ marginBottom: '10px' }} type="primary" shape="round" icon={<DownloadOutlined />}>
                Gerar Relatório .xlsx
            </Button>
        </>


    );
}

export default Home;