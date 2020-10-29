using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NetGL.GraphicsAPI
{
    public static partial class LLGraphics
    {
		[DllImport(OSDetector.GraphicsDLL)]
		public static extern void graphics_setUniformVec2(uint id, float x, float y);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformVec3(uint id, float x, float y, float z);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformVec4(uint id, float x, float y, float z, float w);

		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformdVec2(uint id, double x, double y);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformdVec3(uint id, double x, double y, double z);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformdVec4(uint id, double x, double y, double z, double w);

		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformiVec2(uint id, int x, int y);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformiVec3(uint id, int x, int y, int z);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformiVec4(uint id, int x, int y, int z, int w);

		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformbVec2(uint id, bool x, bool y);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformbVec3(uint id, bool x, bool y, bool z);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformbVec4(uint id, bool x, bool y, bool z, bool w);

		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformMat2(uint id,
			float x1, float x2,
			float y1, float y2
		);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformMat3(uint id,
			float x1, float x2, float x3,
			float y1, float y2, float y3,
			float z1, float z2, float z3
		);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformMat4(uint id,
			float x1, float x2, float x3, float x4,
			float y1, float y2, float y3, float y4,
			float z1, float z2, float z3, float z4,
			float w1, float w2, float w3, float w4
		);

		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformMatd2(uint id,
			double x1, double x2,
			double y1, double y2
		);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformMatd3(uint id,
			double x1, double x2, double x3,
			double y1, double y2, double y3,
			double z1, double z2, double z3
		);
		[DllImport(OSDetector.GraphicsDLL)]
        public static extern void graphics_setUniformMatd4(uint id,
			double x1, double x2, double x3, double x4,
			double y1, double y2, double y3, double y4,
			double z1, double z2, double z3, double z4,
			double w1, double w2, double w3, double w4
		);
	}
}
