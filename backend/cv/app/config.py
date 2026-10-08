import os

USE_MOCK = os.getenv("USE_MOCK", "true").lower() == "true"

# Ограничения на входное изображение (значения согласовать с backend)
MAX_FILE_SIZE_MB = 10
MAX_FILE_SIZE = MAX_FILE_SIZE_MB * 1024 * 1024
ALLOWED_CONTENT_TYPES = {"image/jpeg", "image/png"}
