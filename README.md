Pasos necesarios para iniciar la aplicación (Responde a Cómo ejecutar el proyecto) 

Instalar (si no se tiene instalado) el SDK de .NET 9 mediante el siguiente enlace: 
https://dotnet.microsoft.com/es-es/download/dotnet/thank-you/sdk-9.0.302-windows-x64-installer 

Instalar (si no se tiene instalado) nodejs
https://nodejs.org/dist/v20.17.0/node-v20.17.0-x64.msi

Asegurarse de tener Git instalado 
Abrir una terminal o consola (CMD, PowerShell o Git Bash) y ubicarse en donde se quiere obtener la carpeta del proyecto
Ejecutar
git clone https://github.com/JonattanVarg/ToDoAppJonattanVargas.git
Ubicarse dentro de la carpeta del proyecto
cd ToDoAppJonattanVargas

Podemos abrir la carpeta con visual studio code y usar la terminal, o la de su preferencia
Y ejecutamos para correr la web api .net 9

cd backend/API
dotnet clean
dotnet restore
dotnet build
dotnet watch run

Y finalmente tendremos la API corriendo en http://localhost:5000/swagger/index.html

Ahora ejecutamos las siguientes para correr el cliente angular 18

cd frontend
npm install
npm start

*** Adelanto de la presentación PowerPoint USUARIO INICIAL PARA LOGIN (se pueden registar más si se desea)
Correo Electrónico
adminuser@gmail.com​

Contraseña: 
P@ssw0rd5​

Cómo ejecutar las pruebas > la opción recomendable sería abrir la solución API con visual studio, hacer click derecho en el proyecto API.Tests y seleccionar "Ejecutar Pruebas"

Decisiones técnicas tomadas > Por favor revisar la presentación PowerPoint, allí también se encuentra las credenciales de un usuario listo para poder loguearse en la aplicación

 

