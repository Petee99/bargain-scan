FROM node:latest as node
WORKDIR /app
COPY . .
RUN npm install --force
RUN npm run build --prod

FROM nginx:alpine
COPY nginx-custom.conf /etc/nginx/conf.d/default.conf
COPY --from=node /app/dist/client /usr/share/nginx/html
