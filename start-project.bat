@echo off
echo Iniciando Microservicios...

:: Iniciar Products 
start "Products API" dotnet run --project backend/Products

:: Dar tiempo al primero que cargue
timeout /t 2 /nobreak >nul

:: Iniciar Transactions 
start "Transactions API" dotnet run --project backend/Transactions

echo Servicios iniciados. Puedes minimizar esta ventana.