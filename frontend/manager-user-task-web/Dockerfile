# Use a imagem base Node.js
FROM node:18-alpine

# Define o diretório de trabalho no contêiner
WORKDIR /app

# Copia o arquivo package.json e package-lock.json para o diretório de trabalho
COPY ./frontend/manager-user-task-web/package*.json ./

# Debug: listar arquivos no diretório de trabalho
RUN ls -la /app

# Instala as dependências
RUN npm install

# Copia o restante dos arquivos da aplicação para o diretório de trabalho
COPY . .

# Debug: listar arquivos no diretório de trabalho após copiar
RUN ls -la /app

# Expõe a porta em que a aplicação será executada
EXPOSE 3000

# Comando para iniciar a aplicação
CMD ["npm", "run", "dev"]
