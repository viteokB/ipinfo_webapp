FROM node:alpine as build

# Копируем package.json и package-lock.json
COPY package*.json ./

# Устанавливаем зависимости
RUN npm install

# Копируем все файлы проекта
COPY . .

# Собираем проект
RUN npm run build

FROM nginx:stable-alpine

# Копируем собранные файлы во второй этап
COPY --from=build /build /usr/share/nginx/html

# Копируем конфигурацию nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 7000

CMD ["nginx", "-g", "daemon off;"]