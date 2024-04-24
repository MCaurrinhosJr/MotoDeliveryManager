### Atenção

Importante verificar se os dados do arquivo appsettings foram alterados (no projeto API e RabbitMqConsumer);

### Criação do banco de dados

- Abra o VisualStudio verifique se o projeto selecionado é o API;
- Abra o Package Manager Console no visual studio (View -> Other Windows -> Package Manager Console);
- Selecione o janela do Package Manager Console selecione o projeto "MotoDeliveryManager.Infra";
- Excute o comando "Get-DbContext", será localizado o arquivo "MDMDbContext";
- Após o Arquivo ser localizado execute o comando "Update-Database";
- Caso ocorra erro Build o projeto e repita o passo anterior;

### Para iniciar os projetos

- Para iniciar os projetos pelo visual studio é necessario alterar para iniciar mais de um projeto (Project -> Configure startup projects...);
- Selecione a opção "Multiple Startup Projects" e selecione o Action "Start" para os projetos "MotoDeliveryManager.Api" e "MotoDeliveryManager.RabbitMqConsumer";