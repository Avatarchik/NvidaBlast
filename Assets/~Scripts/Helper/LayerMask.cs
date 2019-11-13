// Only used for the Layers within the Editor
public static class LayerMask
{
	// This performs the 'bit shift the index' operation on the Layer number so you can use it as a layer mask

	// Default Layers
	public static int Default = 1 << 0;
	public static int TransparentFX = 1 << 1;
	public static int IgnoreRaycast = 1 << 2;
	public static int Water = 1 << 3;
	public static int UI = 1 << 4;

	// User Layers
	// Add your own LayerMasks here, '[LayerName] = 1 << [layerNumber]', use the layerNumber in the Editor
	public static int Destructable = 1 << 8;

	// Assigning Layers
	// When assigning layers they cannot be bitshifted
	public static int AssignDefault = 0;
	public static int AssignTransparentFX = 1;
	public static int AssignIgnoreRaycast = 2;
	public static int AssignWater = 3;
	public static int AssignUI = 4;
	public static int AssignDestructable = 8;
}