# Usar uma imagem base com ferramentas de compilação
FROM gcc:latest

# Criar um diretório de trabalho dentro do container
WORKDIR /app

# Copiar o código-fonte do host para o container
COPY . .

# Compilar o código-fonte (substitua "main.cpp" pelo nome do seu arquivo)
RUN g++ -o crono main.cpp

# Definir o comando padrão para executar o aplicativo
CMD ["./crono"]
