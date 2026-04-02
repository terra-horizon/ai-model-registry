from fastapi import UploadFile

from app.schemas.photos import PhotoAnalysisResponse


class PhotoService:
    allowed_content_types = {"image/jpeg", "image/png", "image/webp", }

    async def inspect_photo(self, photo: UploadFile) -> PhotoAnalysisResponse:
        content = await photo.read()
        size_bytes = len(content)
        # Reset stream position so future processing can still read the file later
        await photo.seek(0)
        return PhotoAnalysisResponse(filename=photo.filename, content_type=photo.content_type, size_bytes=size_bytes,
            message="Photo received successfully and is ready for later processing.", )
