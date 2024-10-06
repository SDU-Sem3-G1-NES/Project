


docker build --build-arg POSTGRES_USER=$(grep POSTGRES_USER .env | cut -d '=' -f2) \
             --build-arg POSTGRES_PASSWORD=$(grep POSTGRES_PASSWORD .env | cut -d '=' -f2) \
             --build-arg POSTGRES_DB=$(grep POSTGRES_DB .env | cut -d '=' -f2) \
             -t my_postgres_image .