# Health-Checks

Com o surgimento dos microsserviços a tarefa de verificar se a aplicação está funcionando ou não passou a ser cada vez mais indispensável. Antes utilizava-se muito criar um endpoint que retornava um **statusCode**, isso dizia se a aplicação estava OK ou não. 

Porém, passamos a ter como middleware no ASP.NET Core, mas lembrando a partir da versão 2.2 que fornece um endpoint configurável.

Nessa aplicação utilizei uma API para teste disponiiblizada no endereço https://jsonplaceholder.typicode.com/users

Então, você pode baixar esse repositório, mas se quiser criar o seu próprio Health-Checks vou lhe mostrar em alguns passos.

## 1. Criar aplicação

Para criar através do Visual Studio siga o seguinte caminho e escolha o template **ASP.NET Core Web API**
> File > New > Project 

Agora, se prefere criar pelo terminal só digitar o seguinte comando:

```
dotnet new webapi --framework net5.0
```

## 2. Registrar o serviço

Abra o arquivo **startup.cs**  e no método **ConfigureServices** adicione via interface o seguinte código: **AddHealthChecks()**

```
public void ConfigureServices(IServiceCollection services)
{
  services.AddHealthChecks(); // <-- AQUI

  services.AddControllers();
  services.AddSwaggerGen(c =>
  {
    c.SwaggerDoc("v1", new  OpenApiInfo { Title = "HealthCheck", Version = "v1" });
  });
}
```

## 3. Configurar o middleware e a rota da chamada
Ainda no arquivo **startup.cs**, adicione **UseHealthChecks("/health")** no método **Configure**, porém, na interface **IApplicationBuilder**.

```
public void Configure(IApplicationBuilder  app, IWebHostEnvironment  env)
{
  if (env.IsDevelopment())
  {
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HealthCheck v1"));
  }
  app.UseHttpsRedirection();
  app.UseRouting();  
  app.UseAuthorization();  
  app.UseEndpoints(endpoints =>
  {
    endpoints.MapControllers();
  });
  app.UseHealthChecks("/health"); // <-- AQUI
 }
```

## 4. Vamos utilizar uma interface gráfica
Com essa visão seremos capazes de visualizar informações como histórico, porém, para configurar vamos instalar alguns pacotes necessários:

```
dotnet add package AspNetCore.HealthChecks.UI
dotnet add package AspNetCore.HealthChecks.UI.Client
dotnet add package AspNetCore.HealthChecks.UI.InMemory.Storage
```

Verifica como está seu **ConfigureServices(IServiceCollection services)**:

```
public void ConfigureServices(IServiceCollection services)
{
  services.AddControllers();
  services.AddCors();

  services.AddHealthChecks()
      .AddCheck<CustomHealthChecks>("Health Checks customizavel");

  // Histórico
  services.AddHealthChecksUI(options =>
  {
      options.SetEvaluationTimeInSeconds(5); // Intervalo com que será verificado a aplicação
      options.MaximumHistoryEntriesPerEndpoint(10); // Quantidade máxima registrada no histórico
      
      // Você pode definir o endpoint conforme sua necessidade, neste respositório estamos usando "/health"
      options.AddHealthCheckEndpoint("API com Health Checks", "/health"); // Define o endpoint
  })
  .AddInMemoryStorage();

  services.AddSwaggerGen(c =>
  {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "Health_Checks", Version = "v1" });
  });
}
```

Verifique também o método **Configure()**:

```
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
  if (env.IsDevelopment())
  {
      app.UseDeveloperExceptionPage();
      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Health_Checks v1"));
  }

  app.UseHttpsRedirection();
  app.UseRouting();
  app.UseAuthorization();

  app.UseEndpoints(endpoints =>
  {
      endpoints.MapControllers();
  });

  app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
  {
      Predicate = p => true,
      ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
  });

  app.UseHealthChecksUI(options => { options.UIPath = "/dashboard"; });
}
```

## 5. Executar aplicação
Você pode executar pelo Visual Studio ou acessar o diretório do seu projeto e executá-lo através do terminal com o comando:

```
dotnet run
```

Irá poder testar a API através do Swagger:

![image](https://user-images.githubusercontent.com/22162514/188295583-918a18fa-e3f2-4d62-95eb-50389e9bb492.png)

E para acessar a aplicação experimente o caminho: https://localhost:44308/dashboard

![image](https://user-images.githubusercontent.com/22162514/188295669-721792c2-3691-4510-8e3a-f0ecf4640a67.png)

## Conclusão

No trabalho a gente consome algumas APIs e direto somos questionados se está funcionando ou não, até já desenvolveram uma vez uma aplicação. Porém, para fins didáticos e a título de curiosidade quis desenvolver um, porém, consumindo uma API gratuita para poder compartilhar e permitir que o diretório fique público. Portanto, este repositório ainda poderá sofrer alterações.
