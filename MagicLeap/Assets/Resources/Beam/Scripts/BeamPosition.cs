using UnityEngine;

public class BeamPosition : MonoBehaviour
{
    #region Public Variables
    [Tooltip("The game object of hit effect")]
    public GameObject hitEffect;

    [Tooltip("The direction of the beam")]
    public Vector3 direction;

    [Tooltip("The duration of the beam in second")]
    public float duration;

    [Tooltip("The width of the beam")]
    public AnimationCurve WidthOverTime;

    [Tooltip("The maximum length of the beam")]
    public float maxLength;

    [Tooltip("The speed of the beam moving")]
    public float speed;

    #endregion

    #region Private Variables
    private GameObject endPos;
    private float startTime;
    private float length;
    private ParticleSystem _halo;
    private ParticleSystem _hitGlow;
    private ParticleSystem _hitSpark;
    #endregion

    #region Unity Methods

    void Awake()
    {
        startTime = Time.time;
        endPos = transform.Find("End Point").gameObject;
        endPos.transform.position = transform.position;
        _halo = transform.Find("Halo").gameObject.GetComponent<ParticleSystem>();
        _hitGlow = transform.Find("Hit").transform.Find("Glow").gameObject.GetComponent<ParticleSystem>();
        _hitSpark = transform.Find("Hit").transform.Find("Spark").gameObject.GetComponent<ParticleSystem>();

        // Remove itself
        Destroy(gameObject, duration + 10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time - startTime;

        if (transform.childCount > 0)
        {
            LineRenderer lineRenderer = transform.GetChild(0).gameObject.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position);

            length = maxLength;

            if (currentTime < maxLength / speed)
            {
                length = speed * currentTime;
            }

            endPos.transform.localPosition = new Vector3(0, 0, 0);
            endPos.transform.Translate(direction.normalized * length, Space.Self);

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, length))
            {
                if (!hit.collider.isTrigger)
                {
                    lineRenderer.SetPosition(1, hit.point);

                    hitEffect.SetActive(true);
                    hitEffect.transform.position = hit.point;
                    hitEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
                }
                else
                {
                    lineRenderer.SetPosition(1, endPos.transform.position);

                    hitEffect.SetActive(false);
                }
            }

            else
            {
                lineRenderer.SetPosition(1, endPos.transform.position);

                hitEffect.SetActive(false);
            }

            GameObject glow = transform.GetChild(1).gameObject;
            glow.transform.localRotation = Quaternion.LookRotation(direction);

            lineRenderer.startWidth = Mathf.Clamp(0.5f * WidthOverTime.Evaluate(currentTime / duration) - 0.2f, 0.0f, 1.0f);
            lineRenderer.endWidth = Mathf.Clamp(0.5f * WidthOverTime.Evaluate(currentTime / duration) - 0.2f, 0.0f, 1.0f);

            ParticleSystem.MainModule _haloMain = _halo.main;
            _haloMain.startSize = Random.Range(1.25f, 1.5f) * WidthOverTime.Evaluate(currentTime / duration);

            ParticleSystem.MainModule _hitGlowMain = _hitGlow.main;
            _hitGlowMain.startSize = Random.Range(0.15f, 0.25f) * WidthOverTime.Evaluate(currentTime / duration);

            ParticleSystem.EmissionModule _hitSparkEmission = _hitSpark.emission;
            _hitSparkEmission.rateOverTime = 25 * WidthOverTime.Evaluate(currentTime / duration);
        }
    }

    #endregion
}
