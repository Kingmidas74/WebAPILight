FROM node:14.2.0-alpine3.10 AS build
WORKDIR /usr/src/app
COPY ./ClientApplications/Angular/package.json ./
RUN npm install
COPY ./ClientApplications/Angular .
RUN npm run build

FROM nginx:1.17.10-alpine
COPY --from=build /usr/src/app/dist/TestApp /usr/share/nginx/html
