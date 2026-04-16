# 1. fázis: Build
FROM node:22 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN npx ng build --configuration production

# 2. fázis: Runtime (Nginx)
FROM nginx:stable-alpine
# Az Angular 21 a dist/my-angular-frontend/browser mappába épít
COPY --from=build /app/dist/my-angular-frontend/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
