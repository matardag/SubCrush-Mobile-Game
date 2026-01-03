using UnityEngine;

// Bu sýnýfýn, bir oyun objesine bileþen (Component) olarak eklenebilmesini saðlar.
public class BasitDenizDalgasi : MonoBehaviour
{
    //  Ayarlanabilir Deðiþkenler (Inspector'dan deðiþtirebilirsin)

    [Tooltip("Dalganýn ne kadar yüksek olacaðýný belirler.")]
    public float dalgaYuksekligi = 0.25f; // Baþlangýç deðerini biraz düþürdüm

    [Tooltip("Dalganýn ne kadar hýzlý hareket edeceðini belirler.")]
    public float dalgaHizi = 1f;

    [Tooltip("Dalga yayýlýmýnýn yatay yoðunluðunu ayarlar (X ve Z koordinatlarý).")]
    public float dalgaSikligi = 0.5f;

    //  Ýç Deðiþkenler
    private Mesh mesh;
    private Vector3[] baslangicKoseNoktalari;
    private Vector3[] guncelKoseNoktalari;

    /// <summary>
    /// Nesne ilk yüklendiðinde bir kez çaðrýlýr.
    /// </summary>
    void Start()
    {
        // Mesh bileþenini alýyoruz. Mesh, objenin geometrik þeklinin verisidir.
        mesh = GetComponent<MeshFilter>().mesh;

        // Köþe noktalarýnýn orijinal (sabit) pozisyonlarýný kaydediyoruz.
        baslangicKoseNoktalari = mesh.vertices;

        // Üzerinde çalýþacaðýmýz güncel diziyi orijinalin kopyasý olarak baþlatýyoruz.
        guncelKoseNoktalari = new Vector3[baslangicKoseNoktalari.Length];
        baslangicKoseNoktalari.CopyTo(guncelKoseNoktalari, 0);
    }

    /// <summary>
    /// Her çerçevede (frame) bir kez çaðrýlýr.
    /// </summary>
    void Update()
    {
        // 1. Tüm Köþe Noktalarý Üzerinde Döngü:
        for (int i = 0; i < guncelKoseNoktalari.Length; i++)
        {
            // Köþe noktasýnýn orijinal konumunu alýyoruz (Lokal Uzayda).
            Vector3 orijinalKose = baslangicKoseNoktalari[i];

            // 2. Sinüs Hesaplama (Dalgalanma için):

            // Time.time * dalgaHizi: Dalganýn zamanla ilerlemesini saðlar.
            // (orijinalKose.x + orijinalKose.z) * dalgaSikligi: Dalga yayýlýmýný X ve Z konumuna göre kaydýrýr.
            float sinDegeri = Mathf.Sin(
                Time.time * dalgaHizi +
                (orijinalKose.x + orijinalKose.z) * dalgaSikligi
            );

            // 3. Yeni Yüksekliði Ayarlama:
            float yeniYukseklik = sinDegeri * dalgaYuksekligi;

            // 4. Güncel Köþeyi Güncelleme:
            // Orijinal Y pozisyonuna hesaplanan yeni yüksekliði ekliyoruz.
            guncelKoseNoktalari[i] = new Vector3(
                orijinalKose.x,
                orijinalKose.y + yeniYukseklik,
                orijinalKose.z
            );
        }

        // 5. Mesh'i Güncelleme:
        // Hesapladýðýmýz yeni köþe pozisyonlarýný Mesh'e geri atýyoruz.
        mesh.vertices = guncelKoseNoktalari;

        // Iþýk ve gölgelerin doðru görünmesi için normal vektörlerini yeniden hesapla.
        mesh.RecalculateNormals();
    }
}