# CaioOliveira.Cep.ViaCep

Este serviço tem por objetivo possibilitar a consulta de CEP utilizando como fonte dos dados a API do ViaCep.

Para utilizar é muito simples, basta configurar sua classe Startup conforme o exemplo abaixo:

```c#
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.UseViaCep();
    ...    
}
```

Feito isso, basta solicitar em qualquer classe que seja injetada pela injeção de dependência do ASP.NET Core, conforme exemplo abaixo:

```c#
public class FooAppService
{
    private readonly ICepFinderService _cepFinder;

    public FooAppService(ICepFinderService cepFinder)
    {
        _cepFinder = cepFinder;   
    }
    
    public async Task GetCity()
    {
        ...
        var address = await cepFinder.Find("02033000");
        ...
    }
}
```