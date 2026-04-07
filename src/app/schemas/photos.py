from pydantic import BaseModel


class PhotoAnalysisResponse(BaseModel):
    filename: str
    content_type: str | None
    size_bytes: int
    message: str

class PhotoBase64Request(BaseModel):
    image_base64: str
    filename: str | None = None