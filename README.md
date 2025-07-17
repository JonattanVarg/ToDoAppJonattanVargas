Pasos necesarios para iniciar la aplicación (Responde a Cómo ejecutar el proyecto) 

Instalar el SDK de .NET 9 mediante el siguiente enlace: 
https://dotnet.microsoft.com/es-es/download/dotnet/thank-you/sdk-9.0.302-windows-x64-installer 

Clonar el proyecto usando la URL
https://github.com/JonattanVarg/ToDoAppJonattanVargas.git

Podemos abrir la carpeta con visual studio code o la preferencia
Y ejecutamos para correr la web api .net 9

cd backend/API
dotnet clean
dotnet restore
dotnet build
dotnet watch run

Y ejecutamos las siguientes para correr el cliente angular 18

cd frontend
npm install
npm start

Cómo ejecutar las pruebas > la opción recomendable sería abrir la solución API con visual studio, hacer click derecho en el proyecto API.Tests y seleccionar "Ejecutar Pruebas"

Decisiones técnicas tomadas > Por favor revisar la presentación PowerPoint, allí también se encuentra las credenciales de un usuario listo para poder loguearse en la aplicación

 

