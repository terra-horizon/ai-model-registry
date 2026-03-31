from pydantic import BaseModel


class PhotoAnalysisResponse(BaseModel):
    filename: str
    content_type: str | None
    size_bytes: int
    message: str
