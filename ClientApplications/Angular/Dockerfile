FROM node:14.2.0-alpine3.10 AS build
WORKDIR /usr/src/app
COPY package.json ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:1.17.10-alpine
COPY --from=build /usr/src/app/dist/TestApp /usr/share/nginx/html
