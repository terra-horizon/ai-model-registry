import base64

from fastapi import HTTPException

from app.schemas.photos import PhotoAnalysisResponse, PhotoBase64Request


class PhotoService:
    allowed_content_types = {"image/jpeg", "image/png", "image/webp", }

    def _detect_content_type(self, data: bytes) -> str | None:
        if data.startswith(b"\xff\xd8"):
            return "image/jpeg"
        if data.startswith(b"\x89PNG"):
            return "image/png"
        if data.startswith(b"RIFF") and b"WEBP" in data[:16]:
            return "image/webp"
        return None

    async def inspect_photo(self, request: PhotoBase64Request) -> PhotoAnalysisResponse:
        try:
            image_bytes = base64.b64decode(request.image_base64, validate=True)
        except Exception:
            raise HTTPException(status_code=400, detail="Invalid base64 string.")

        if len(image_bytes) == 0:
            raise HTTPException(status_code=400, detail="Empty image.")
        content_type = self._detect_content_type(image_bytes)
        if content_type not in self.allowed_content_types:
            raise HTTPException(status_code=400, detail="Unsupported image format (only JPEG, PNG, WEBP allowed).", )
        return PhotoAnalysisResponse(
            filename=request.filename or "unknown",
            content_type=content_type,
            size_bytes=len(image_bytes),
            message="Photo received successfully and is ready for processing.",
        )
