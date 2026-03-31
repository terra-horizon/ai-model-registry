from fastapi import HTTPException, UploadFile

from schemas.photos import PhotoAnalysisResponse


class PhotoService:
    allowed_content_types = {"image/jpeg", "image/png", "image/webp", }

    async def inspect_photo(self, photo: UploadFile) -> PhotoAnalysisResponse:
        if not photo.filename:
            raise HTTPException(status_code=400, detail="A file name is required.")

        if photo.content_type not in self.allowed_content_types:
            raise HTTPException(status_code=400, detail="Only JPEG, PNG, and WEBP images are allowed.", )

        content = await photo.read()
        size_bytes = len(content)

        if size_bytes == 0:
            raise HTTPException(status_code=400, detail="The uploaded file is empty.")

        # Reset stream position so future processing can still read the file later
        await photo.seek(0)

        return PhotoAnalysisResponse(filename=photo.filename, content_type=photo.content_type, size_bytes=size_bytes,
            message="Photo received successfully and is ready for later processing.", )
