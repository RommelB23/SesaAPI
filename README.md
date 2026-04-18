# SesaAPI
Prueba Técnica: Sistema de Emisión de Seguros de Auto (SESA)

Pasos para poner a prueba el proyecto:

1. Habiendo descargado el repositorio, para poder probar las APIs lo primero que necesitan es ejecutar el script del archivo SesaDB.sql en SQL Server que se encuentra dentro de la carpeta scripts. Se crearán las tablas y algunos registros para clientes, vehículos y tipos de cobertura que servirán para emitir las pólizas. La base de datos fue creada mediante code first, pero para facilitar las pruebas solo deben ejecutar el script y no correr ninguna migración.
2.  Con la base de datos creada deben abrir el archivo de la solución (SesaAPI.slnx) usando Visual Studio 2016, después hacer clic derecho en el proyecto SesaAPI y selecionar "Establecer como proyecto de inicio". Con esto ya pueden ejecutar el proyecto, ya sea en https, http o IIS Express. Si usan Visual Studio Code, tendrán que abrir el cmd y entrar en src/SesaAPI/ para ejecutar el comando dotnet run, pero asegurarse de que cuando se levante el localhost se agregue al final /swagger para poder probar los endpoints. El enlace debería ser algo así http://localhost:5100/swagger/index.html
3.  En el proyecto SesaAPI deben revisar el archivo appsettings.json y asegurarse de que en la cadena de conexión el server sea localhost\SQLEXPRESS o lo editan en caso de que su instancia local de SQL Server tenga otro nombre.
4.  En cuanto se ejecute el proyecto se abrirá una página del navegador con una pantalla de Swagger para que puedan probar los endpoints.

El proyecto es una aplicación ASP.NET Core Web API con lenguaje C# y el framework .NET 8.0, Entity Framework Core (Code First) y SQL Server para la base de datos.
