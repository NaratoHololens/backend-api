using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dto
{
    public class DetectedPersonDto
    {
        public string personId { get; set; }
        public string faceId { get; set; }
        public Candidate[] candidates { get; set; }
        public FaceRectangle faceRectangle { get; set; }

    }
}
