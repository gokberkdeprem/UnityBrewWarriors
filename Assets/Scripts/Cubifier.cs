// using System.Collections;

using UnityEngine;

public class Cubifier : MonoBehaviour
{
    public GameObject targetCube;

    public Vector3 sectionCount;
    public Material subCubeMaterial;
    private Vector3 _fillStartPosition;
    private Transform _parentTransform;
    private Vector3 _sectionSize;

    private Vector3 _sizeOfOriginalCube;
    private GameObject _subCube;

    private void Start()
    {
        if (targetCube == null)
            targetCube = gameObject;

        _sizeOfOriginalCube = targetCube.transform.lossyScale;
        _sectionSize = new Vector3(
            _sizeOfOriginalCube.x / sectionCount.x,
            _sizeOfOriginalCube.y / sectionCount.y,
            _sizeOfOriginalCube.z / sectionCount.z
        );

        _fillStartPosition = targetCube.transform.TransformPoint(new Vector3(-0.5f, 0.5f, -0.5f))
                             + targetCube.transform.TransformDirection(new Vector3(_sectionSize.x, -_sectionSize.y,
                                 _sectionSize.z) / 2.0f);

        _parentTransform = new GameObject(targetCube.name + "CubeParent").transform;
        // StartCoroutine(DivideIntoCuboids());
        // InstantDivideIntoCuboids();
    }

    public void InstantDivideIntoCuboids()
    {
        for (var i = 0; i < sectionCount.x; i++)
        for (var j = 0; j < sectionCount.y; j++)
        for (var k = 0; k < sectionCount.z; k++)
        {
            _subCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _subCube.transform.localScale = _sectionSize;
            _subCube.transform.position = _fillStartPosition +
                                          targetCube.transform.TransformDirection(new Vector3(_sectionSize.x * i,
                                              -_sectionSize.y * j, _sectionSize.z * k));
            _subCube.transform.rotation = targetCube.transform.rotation;

            _subCube.transform.SetParent(_parentTransform);
            _subCube.GetComponent<MeshRenderer>().material = subCubeMaterial;
        }

        Destroy(targetCube);
        foreach (Transform subCuboid in _parentTransform) subCuboid.gameObject.AddComponent<Rigidbody>();

        //Looper();
    }

    #region AnimatedAppear

    // private void Looper()
    // {
    //     Destroy(_parentTransform.gameObject);
    //     _parentTransform = new GameObject(targetCube.name + "Parent").transform;
    //
    //     StartCoroutine(DivideIntoCuboids());
    // }

    // private IEnumerator DivideIntoCuboids()
    // {
    //     for (var i = 0; i < sectionCount.x; i++)
    //     for (var j = 0; j < sectionCount.y; j++)
    //     for (var k = 0; k < sectionCount.z; k++)
    //     {
    //         _subCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //
    //         _subCube.transform.localScale = _sectionSize;
    //         _subCube.transform.position = _fillStartPosition +
    //                                       targetCube.transform.TransformDirection(new Vector3(_sectionSize.x * i,
    //                                           -_sectionSize.y * j, _sectionSize.z * k));
    //         _subCube.transform.rotation = targetCube.transform.rotation;
    //
    //         _subCube.transform.SetParent(_parentTransform);
    //         _subCube.GetComponent<MeshRenderer>().material = subCubeMaterial;
    //
    //         yield return new WaitForSeconds(0.1f);
    //     }
    //
    //     Destroy(targetCube);
    //
    //
    //     // foreach (Transform subCube in ParentTransform)
    //     // {
    //     // subCube.gameObject.AddComponent<Rigidbody>();
    //     // }
    //
    //     //Looper();
    // }

    #endregion
}