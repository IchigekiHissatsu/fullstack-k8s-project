# 1. fázis: Build
FROM node:22 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
# Kényszerítjük a dist mappát almappák nélkül
RUN npx ng build --configuration production --output-path=dist

# 2. fázis: Runtime (Nginx)
FROM nginx:stable-alpine
# Az Angular 17+ a 'browser' almappába teszi a fájlokat a dist-en belül
COPY --from=build /app/dist/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]