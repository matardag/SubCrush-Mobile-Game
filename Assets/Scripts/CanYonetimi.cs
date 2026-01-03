using UnityEngine;

// Sýnýf adýný CanYonetimi olarak deðiþtiriyoruz.
public class CanYonetimi : MonoBehaviour
{
    [Tooltip("Objenin baþlangýçtaki maksimum caný.")]
    public float maksimumCan = 100f;

    // Anlýk can deðeri
    private float mevcutCan;

    void Start()
    {
        // Oyuna baþlarken caný maksimum deðere ayarla
        mevcutCan = maksimumCan;

        // Caný düzenli aralýklarla konsola yazdýrmak için InvokeRepeating kullanýyoruz.
        // 0 saniye sonra baþla ve her 1 saniyede bir tekrarla.
        InvokeRepeating("KalanCaniYazdir", 0f, 1f);
    }

    /// <summary>
    /// Objenin anlýk canýný Unity Konsoluna yazdýrýr.
    /// </summary>
    void KalanCaniYazdir()
    {
        // Debug.Log'da objenin adýný da gösterelim ki hangi gemiye ait olduðu belli olsun
        Debug.Log(gameObject.name + " Caný: " + mevcutCan + " / " + maksimumCan);
    }

    /// <summary>
    /// Caný azaltmak veya artýrmak için dýþarýdan çaðrýlacak metot.
    /// </summary>
    /// <param name="miktar">Can deðiþimi miktarý. Pozitif ise artýþ, negatif ise azalýþ.</param>
    public void CaniDegistir(float miktar)
    {
        // Caný güncelle
        mevcutCan += miktar;

        // Canýn sýfýrýn altýna düþmesini engelle
        if (mevcutCan <= 0)
        {
            mevcutCan = 0;
            Debug.Log(gameObject.name + " yok edildi!");
            // Örneðin: Destroy(gameObject); 
        }

        // Canýn maksimum deðeri aþmasýný engelle
        if (mevcutCan > maksimumCan)
        {
            mevcutCan = maksimumCan;
        }

        // Can deðiþtiðinde de konsola yazdýr 
        KalanCaniYazdir();
    }
}