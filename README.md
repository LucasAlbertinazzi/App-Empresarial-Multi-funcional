# App Empresarial Multi-funcional

Um aplicativo mobile desenvolvido em .NET MAUI destinado a oferecer uma série de funcionalidades para otimizar e facilitar operações empresariais. Atualmente, o app conta com um sistema de liberação de pedidos para indivíduos com restrições financeiras, como score baixo, dívidas, entre outros. No entanto, estão planejadas diversas outras funcionalidades para futuras atualizações.

## Funcionalidades Atuais:
- **Liberação de Pedidos**: Facilita a aprovação de pedidos para usuários com diferentes níveis de restrições financeiras.

## Funcionalidades Planejadas:
- **Consulta de Clientes**: Pesquisa e visualização de informações de clientes.
- **Vendas pelo App**: Facilitar o processo de vendas diretamente pelo aplicativo.
- **Setor Financeiro**: Gerenciamento e visualização de informações financeiras.
- **Rotas de Entrega**: Planejamento e visualização de rotas de entrega de produtos.
- ... entre outras.

## Tecnologias e Frameworks:
- **Linguagem**: C#.
- **Plataforma**: .NET MAUI.
- **Plataformas-alvo**: Inicialmente apenas para Android.
- **Dependências Principais**:
  - `Microsoft.AspNet.WebApi.Client`: Para chamadas de API.
  - `Microsoft.EntityFrameworkCore`: Operações relacionadas a bancos de dados.
  - `Microsoft.Extensions.Logging.Debug`: Para logs e depuração.
  - `Plugin.LocalNotification`: Para notificações locais.
  - `Xamarin.Essentials`: Funcionalidades essenciais multiplataforma.

## Estrutura do Projeto:
- **Views**: Gerenciamento de cobrança e interface principal.
- **Classes e Modelos**: Diretoria, Auditoria e Principal.
- **Serviços e ViewModels**: Auditoria, Cobrança e Diretoria.
