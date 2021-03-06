import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Link } from "react-router-dom";
import {
  MenuFoldOutlined,
  MenuUnfoldOutlined,
  TeamOutlined,
  VideoCameraOutlined,
  FormOutlined,
  HomeOutlined
} from '@ant-design/icons';
import { Layout, Menu } from 'antd';

import React, { useState } from 'react';
import './App.css';

import Cliente from './Cliente'
import Filme from './Filme'
import Locacao from './Locacao'
import Home from './Home'
const { Header, Sider, Content } = Layout;

function getItem(label, key, icon, children) {
  return {
    key,
    icon,
    children,
    label,
  };
}

const items = [
  getItem(
    <><span>Home</span><Link to="/" /></>,
    'Home',
    <HomeOutlined />,
  ),
  getItem(
    <><span>Cliente</span><Link to="/cliente" /></>,
    'Cliente',
    <TeamOutlined />,
  ),
  getItem(
    <><span>Filmes</span><Link to="/filme" /></>,
    'Filme',
    <VideoCameraOutlined />,
  ),
  getItem(
    <><span>Locacao</span><Link to="/locacao" /></>,
    'Locacao',
    <FormOutlined />,
  ),
];

function App() {
  const [collapsed, setCollapsed] = useState(false);
  return (
    <BrowserRouter>
      <Layout style={{ minHeight: "100vh", height: "100%" }}>
        <Sider trigger={null} collapsible collapsed={collapsed}>
          <div className="logo" />
          <Menu
            theme="dark"
            mode="inline"
            items={items}
          >
          </Menu>
        </Sider>
        <Layout className="site-layout">
          <Header
            className="site-layout-background"
            style={{
              padding: 0,
            }}
          >
            {React.createElement(collapsed ? MenuUnfoldOutlined : MenuFoldOutlined, {
              className: 'trigger',
              onClick: () => setCollapsed(!collapsed),
            })}
          </Header>
          <Content
            className="site-layout-background"
            style={{
              margin: '24px 16px',
              padding: 24,
              minHeight: 280,
            }}
          >
            <Routes>
              <Route path="cliente" element={<Cliente />} />
              <Route path="filme" element={<Filme />} />
              <Route path="locacao" element={<Locacao />} />
              <Route path="/" element={<Home />} />
            </Routes>

          </Content>
        </Layout>
      </Layout>
    </BrowserRouter>

  );
}

export default App;