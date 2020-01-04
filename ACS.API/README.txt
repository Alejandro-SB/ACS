#TO RUN:
Open CMD in root

docker build -t acs.api .
docker run -d -p 8080:80 --name acswebapi acs.api