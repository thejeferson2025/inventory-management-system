FROM node:20
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
EXPOSE 4200
# El flag --host 0.0.0.0 es obligatorio en Docker para que Angular sea accesible desde tu navegador
CMD ["npm", "run", "start", "--", "--host", "0.0.0.0"]
