using Dynamite3D.RealIvy;
using UnityEngine;

public class IvySpawner : MonoBehaviour
{
    public IvyCaster ivyCaster;

    // Start is called before the first frame update
    void Start()
    {
        ivyCaster.CastRandomIvy(transform.position, transform.rotation);
    }

}
