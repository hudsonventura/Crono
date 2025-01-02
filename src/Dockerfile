# Usar uma imagem base com ferramentas de compilação
FROM gcc:latest

# Criar um diretório de trabalho dentro do container
WORKDIR /app

# Copiar o código-fonte do host para o container
COPY . .

# Instalar a biblioteca libyaml-cpp-dev
RUN apt update && apt install -y libyaml-cpp-dev libjsoncpp-dev


# Compilar o código-fonte (substitua "main.cpp" pelo nome do seu arquivo)
RUN g++ main.cpp Yaml.cpp Container.cpp Docker.cpp Cron.cpp Console.cpp $(pkg-config --cflags --libs jsoncpp yaml-cpp) -o crono

# Definir o comando padrão para executar o aplicativo
CMD ["./crono"]
# g++ main.cpp Yaml.cpp Container.cpp Docker.cpp Cron.cpp Console.cpp $(pkg-config --cflags --libs jsoncpp yaml-cpp) -o output/crono
# watch "ps -C crono -o rss | awk 'NR>1 {print \$1/1024 \" MB\"}'"
# watch "ps -C crono -o rss | awk 'NR>1 {print \$1 \" KB\"}'"




