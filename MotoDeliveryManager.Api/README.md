### Aten��o

Importante verificar se os dados do arquivo appsettings foram alterados (no projeto API e RabbitMqConsumer);

### Cria��o do banco de dados

- Abra o VisualStudio verifique se o projeto selecionado � o API;
- Abra o Package Manager Console no visual studio (View -> Other Windows -> Package Manager Console);
- Selecione o janela do Package Manager Console selecione o projeto "MotoDeliveryManager.Infra";
- Excute o comando "Get-DbContext", ser� localizado o arquivo "MDMDbContext";
- Ap�s o Arquivo ser localizado execute o comando "Update-Database";
- Caso ocorra erro Build o projeto e repita o passo anterior;