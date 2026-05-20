using UnityEngine;

public class FlorescenteView : MonoBehaviour
{
    [SerializeField] private float velocidadeOscilacao = 1.5f;
    [SerializeField] private float amplitudeOscilacao  = 0.15f;

    private Vector3 posicaoInicial;

    private void Start()
    {
        posicaoInicial = transform.position;
    }

    private void Update()
    {
        float offsetY = Mathf.Sin(Time.time * velocidadeOscilacao) * amplitudeOscilacao;
        transform.position = posicaoInicial + new Vector3(0f, offsetY, 0f);
        transform.Rotate(Vector3.up, 45f * Time.deltaTime, Space.World);
    }

    public void Coletar()
    {
        gameObject.SetActive(false);
    }
}
