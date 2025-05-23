# Projeto Academia WPF

Desenvolvi uma aplicação desktop completa utilizando C# com WPF para o gerenciamento de academias. A aplicação é estruturada com o padrão **MVVM (Model-View-ViewModel)** para garantir organização, manutenibilidade e separação clara de responsabilidades.

## Sobre o Projeto

Este sistema oferece funcionalidades robustas para o controle e gerenciamento de diversas entidades em uma academia, incluindo:

* **Cadastro e Controle de Alunos**: Gerenciamento completo de informações de alunos, incluindo dados pessoais e foto.
* **Cadastro e Controle de Colaboradores**: Administração de dados de funcionários, com diferentes tipos (Administrador, Atendente, Instrutor) e vínculos (CLT, Estágio).
* **Matrículas**: Gerenciamento de planos de matrícula (Mensal, Trimestral, Semestral, Anual), datas de início e fim, restrições médicas, observações e upload de laudos.
* **Endereços (Logradouros)**: Cadastro e consulta de informações de logradouros, utilizados para vincular endereços a alunos e colaboradores.
* **Avaliações Físicas**: Módulos para cadastro de avaliações (com campos para peso, altura, medidas corporais, etc.).
* **Frequência**: Registro de entrada e saída de alunos na academia.
* **Aulas e Treinos**: Páginas dedicadas para gerenciamento de aulas e treinos.

A interface é moderna, desenvolvida em XAML, e o sistema possui suporte a **internacionalização**, permitindo a troca dinâmica de idioma (atualmente configurado para `en-US` e `pt-BR`).

A camada de acesso a dados (`DataAccess`) utiliza o padrão **Repository**, encapsulando a lógica de persistência e tratamento de dados.

## Tecnologias Utilizadas

* **C#**: Linguagem de programação principal.
* **WPF (Windows Presentation Foundation)**: Para o desenvolvimento da interface gráfica desktop.
* **MVVM (Model-View-ViewModel)**: Padrão de arquitetura para organizar o código e a lógica da aplicação.
* **MySQL**: Sistema de gerenciamento de banco de dados (o projeto está configurado para `MySql.Data.MySqlClient`).
* **iText7**: Biblioteca para geração de relatórios em PDF (alunos, matrículas, logradouros, colaboradores).

## Funcionalidades Principais

* **CRUD Completo**: Operações de Criar, Ler, Atualizar e Deletar para Alunos, Colaboradores, Matrículas e Logradouros.
* **Internacionalização**: Suporte a múltiplos idiomas com recursos dinâmicos (via `Properties.Idioma` e `ClassFuncoes.AjustaResources`).
* **Validação de Conexão com Banco de Dados**: Verifica a conexão com o MySQL e permite configuração em caso de falha.
* **Máscaras de Entrada**: Campos de texto com máscaras para CPF, CEP, Telefone, Data, Hora, Peso e Altura.
* **Geração de Relatórios em PDF**: Capacidade de gerar relatórios detalhados para Alunos, Colaboradores, Matrículas e Logradouros.
* **Sistema de Login com Permissões**: Diferentes níveis de acesso (Administrador, Atendente, Instrutor) que habilitam/desabilitam funcionalidades da interface.
* **Conversores de Dados**: Inclui conversores para `BooleanToVisibility` e `ByteArrayToImage` para manipulação de UI e exibição de imagens.
