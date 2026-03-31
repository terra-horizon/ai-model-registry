from services.photo_service import PhotoService


def get_photo_service() -> PhotoService:
    return PhotoService()