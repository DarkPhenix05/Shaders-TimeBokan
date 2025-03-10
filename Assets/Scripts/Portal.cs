using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Camera _camera;

    public GameObject _portal2;
    public Transform _TPpos;
    public Portal _portal2Script;

    public Vector3 _deltaPPp;
    public Quaternion _deltaPPr;

    public Transform _playerPos;
    public Transform _portalPos;
    public Camera _playerCamera;

    public GameObject _playerRoot;
    public GameObject _playerModel;
    public Player _playerScript;


    public bool _active = true;

    public bool _calculateToCamera = false; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _camera = this.gameObject.GetComponentInChildren<Camera>();

        if(_portal2 != null)
        {
            _TPpos = _portal2.transform.GetChild(0);
            _portal2Script = _portal2.GetComponent<Portal>();
        }
        else
        {
            Warning.Error(this.gameObject + " doesn´t have a _portal2");
        }

        _playerRoot = FindFirstObjectByType<Player>().gameObject;

        _playerScript = _playerRoot.GetComponent<Player>();
        _playerCamera = Camera.main;
        _playerModel = _playerRoot.transform.GetChild(0).gameObject;
        _portalPos = this.gameObject.transform;

        if (_calculateToCamera)
        {
            _playerPos = _playerCamera.transform;
        }
        else
        {
            _playerPos = _playerRoot.transform;
        }
    }

    private void FixedUpdate()
    {
        if (_calculateToCamera)
        {
            _deltaPPp = new Vector3(_portalPos.position.x - _playerPos.position.x, 
                                    _playerPos.position.y - _portalPos.position.y, 
                                    _portalPos.position.z - _playerPos.position.z);

            _deltaPPr = Quaternion.Euler(new Vector3(
                                        _portalPos.rotation.eulerAngles.x - _playerPos.rotation.eulerAngles.x + 180,
                                        _playerPos.rotation.eulerAngles.y - _portalPos.rotation.eulerAngles.y,
                                        _portalPos.rotation.eulerAngles.z - _playerPos.rotation.eulerAngles.z + 180));
        }
        else
        {
            _deltaPPp = new Vector3(_portalPos.position.x - _playerPos.position.x,
                                    _portalPos.position.y - _playerPos.position.y,
                                    _portalPos.position.z - _playerPos.position.z);

            _deltaPPr = Quaternion.Euler(new Vector3(
                                    _portalPos.rotation.eulerAngles.x - _playerPos.rotation.eulerAngles.x + 180,
                                    _portalPos.rotation.eulerAngles.y - _playerPos.rotation.eulerAngles.y,
                                    _portalPos.rotation.eulerAngles.z - _playerPos.rotation.eulerAngles.z + 180));
        }

        MoveCamera(_deltaPPp, _deltaPPr);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangeVewTarget();
        }
    }

    public void TeleportTo(bool player, GameObject colObject = null)
    {
        if(!_TPpos)
        {
            Debug.Log("NO TP");
            return;
        }
        if(player)
        {
            _playerModel.SetActive(false);
            
            _portal2Script.CoolDown();

            _playerPos.position = _TPpos.position;
            _playerPos.rotation = _TPpos.rotation;
            _playerCamera.transform.rotation = _TPpos.rotation;
            Debug.LogWarning(_playerCamera.transform.rotation);

            _playerModel.SetActive(true);
        }
        else if(colObject)
        {
            colObject.SetActive(false);
            colObject.transform.position = _TPpos.position;
            colObject.transform.rotation = _TPpos.rotation;
            colObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!_active)
        { 
            return; 
        }

        if(collision.gameObject.layer.Equals(3))
        {
            //Debug.LogWarning("IsPlayer: " + collision.gameObject.layer);
            TeleportTo(true);
        }
        else
        {
            //Debug.LogWarning("Collided: " + collision.gameObject.layer);
            TeleportTo(false, collision.gameObject);
        }
    }

    public void CoolDown()
    {
        StartCoroutine(DeactovatePortal());
    }

    public IEnumerator DeactovatePortal()
    {
        _active = false;
        yield return new WaitForSecondsRealtime(1.0f);
        _active = true;
    }

    public void MoveCamera(Vector3 pos, Quaternion rot)
    {
        _camera.transform.position = pos;
        _camera.transform.rotation = rot;
    }

    public void SetTPpos(Transform TPpos)
    {
        _TPpos = TPpos;
    }

    public void ChangeVewTarget()
    {
        _calculateToCamera = !_calculateToCamera;

        if(_calculateToCamera) 
        {
            _playerPos = _playerCamera.transform;
        }
        else
        {
            _playerPos = _playerRoot.transform;
        }
    }
}
