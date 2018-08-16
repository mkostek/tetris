/*
 * SharpDevelop tarafından düzenlendi.
 * Kullanıcı: abuzer
 * Tarih: 14.8.2018
 * Zaman: 17:27
 * 
 * Bu şablonu değiştirmek için Araçlar | Seçenekler | Kodlama | Standart Başlıkları Düzenle 'yi kullanın.
 */
using System;
using System.Timers;
namespace tetris
{
	class Program
	{
		public static int en=15;
		public static int boy=30;//haritanın en ve boyunu ayarlayan parametre
		public static int say=0;//blok sayısı
		public static int zaman=0,zamani=-1;//timerde gecen zamanı tutmak için tanımlanmıs degerler
		public static int[,] blok=new int[4,2];//bir bloğu devriğini almak için kordinatlarının tututldğu dizi tipi değişkenkler
		private static int[,] harita = new int[boy, boy];//haritayı tanımla
		private static Timer t = new Timer(50);//timer i set ediyoruz
		public static void kontrol()//oyun bittimi yada satır dolumu kontrolu
		{
			int sayac=0;
			for(int j=boy-1;j>0;j--)
			{
				for(int k=en-1;k>0;k--)
				{
					if(harita[j,k]==5)
					{
						sayac++;
					}
				}
				if(sayac==en-2){//birsatırda tüm kolonlar dolduysa satırı kaydır üsttekiler gelsin
					for(int h=j;h>3;h--){
						for(int k=1;k<en-1;k++)
						{
							harita[h,k]=harita[h-1,k];
						}
					}
					
				}sayac=0;
			}
				for(int k=1;k<en-1;k++)//en tepeye kadar 5 nolu blok geldiyse game over
					if(harita[1,k]==5){
					t.Stop();
					Console.Write("game over...");
						
			}
		}
		public static void rastgele()//rastgele blok gönder
		{
			Random r=new Random();
			switch(r.Next(0,8))
			{
				case 0:
					harita[1,en/2]=4;
					harita[1,en/2+1]=4;
					harita[2,en/2+1]=4;
					harita[2,en/2+2]=4;
					break;
				case 1:
					harita[1,en/2]=4;
					harita[1,en/2+1]=4;
					harita[2,en/2]=4;
					harita[2,en/2+1]=4;
					break;
				case 2:
					harita[1,en/2+1]=4;
					harita[2,en/2]=4;
					harita[2,en/2+1]=4;
					harita[2,en/2+2]=4;
					break;
				case 3:
					harita[1,en/2-2]=4;
					harita[1,en/2-1]=4;
					harita[1,en/2]=4;
					harita[1,en/2+1]=4;
					break;
				case 4:
					harita[1,en/2-1]=4;
					harita[2,en/2-1]=4;
					break;
				case 5:
					harita[1,en/2]=4;
					harita[2,en/2]=4;
					harita[3,en/2]=4;
					harita[2,en/2+1]=4;
					break;
				case 6:
					harita[1,en/2]=4;
					harita[2,en/2]=4;
					harita[3,en/2]=4;
					harita[2,en/2-1]=4;
					break;
				case 7:
					harita[2,en/2]=4;
					harita[2,en/2+1]=4;
					harita[1,en/2+1]=4;
					harita[1,en/2+2]=4;
					break;
				case 8:
					harita[2,en/2+1]=4;
					harita[1,en/2]=4;
					harita[1,en/2+1]=4;
					harita[1,en/2+2]=4;
					break;
			}
		}
		public static bool engel(char u){//blokların yerleştirilmesinde okunan değerlere göre engel varmı yokmu
			bool flag=true;
			if(u=='4'){//sola doğru hareket varsa
				for(int k=0;k<boy-1;k++)
				{
					for(int l=0;l<en-1;l++)
					{
						if(harita[k,l]==4 && (harita[k+1,l-1]==3 || harita[k+1,l-1]==5 || harita[k,l-1]==5)){//3 ve 5 değilse 
							flag=false;
						}
					}
				}
			}else if(u=='6'){
				for(int k=boy-1;k>0;k--)
				{
					for(int l=en-1;l>0;l--)
					{
						if(harita[k,l]==4 && (harita[k+1,l+1]==3 || harita[k+1,l+1]==5 || harita[k,l+1]==5)){//3 ve 5 değilse 
							flag=false;
						}
					}
				}
			}else if(u=='8'){//zemine oturma kontrolu
				for(int k=0;k<boy-1;k++)
				{
					for(int l=0;l<en-1;l++){
						if(harita[k,l]==4 && (harita[k+1,l]==5 || harita[k+1,l]==3))
						{
							flag=false;
						}
					}
				}
			}else if(u=='5'){//bloğun devriğini alma kontrolu
				int say=0;
				int c=0;
				for(int k=0;k<boy-1;k++)
				{
					for(int j=0;j<en-1;j++)
					{
						if(harita[k,j]==4)
						{
							if(say==0){
								c=j-k;
							}
							blok[say,0]=k;
							blok[say,1]=j;
							
							say++;
						}
					}
				}
				for(int ı=0;ı<say;ı++){//döndürürken sınırlara denk geliyorsa yada herhangi tuğla var mı
					if(harita[blok[ı,1]-c,blok[ı,0]+c]==3 || harita[blok[ı,1]-c,blok[ı,0]+c]==5)
						flag=false;//var sa hop
				}
				say=0;
			}
			return flag;//en nsonda bayrağı döndür
			
		}
		private static void hareket(object o, ElapsedEventArgs a)
		{
			zaman++;
			yazdır();//önce bir haritayı yazdır
			Console.WriteLine(zaman);
			char b='6';
			if(zamani!=zaman){
				zamani=zaman;//zamanı yeni değere set et
				if(engel('8'))//zeminin oturmasına engel yoksa
				{
					for(int i=boy-1;i>0;i--)
					{
						for(int j=en-1;j>0;j--)
						{

							if(harita[i,j]==4)
							{
								harita[i,j]=0;//blogu aşağı ötele
								harita[i+1,j]=4;
							}

						}
						
					}
				}else//engel varsa
				{
					for(int k=0;k<boy-1;k++)
					{
						for(int l=0;l<en-1;l++)
						{
							if(harita[k,l]==4)
							{
								harita[k,l]=5;
								rastgele();//rastgele yeni bir blok üret
								kontrol();//kontrol et satırda tüm dolu olan varsa yoket
								
							}
						}
					}
					
				}
			}
			b=Console.ReadKey(true).KeyChar;//klavye oku
			switch(b)
			{
				case '5':
					if(engel(b))//blogu döndürmeye engel var mı?
					{//yoksa
						int c=0;
						for(int k=0;k<boy-1;k++)
						{
							for(int j=0;j<en-1;j++)
							{
								if(harita[k,j]==4)
								{
									if(say==0){
										c=j-k;
									}
									harita[k,j]=0;
									blok[say,0]=k;//transpozu nu al
									blok[say,1]=j;
									
									say++;
								}
							}
						}
						for(int ı=0;ı<say;ı++){
							harita[blok[ı,1]-c,blok[ı,0]+c]=4;// ve yeni bloku güncelle
						}
						say=0;
						
					}break;
				case '6':
					if(engel(b))//sağa dogru hareket etmesine engel varmı
						for(int k=boy-1;k>0;k--)//yoksa
					{
						for(int l=en-1;l>0;l--){
							if(harita[k,l]==4 )
							{
								harita[k,l]=0;
								harita[k,l+1]=4;//tüm blogu saga dogru kaydır
							}
						}
					}break;
				case '4':
					if(engel(b))//blogun sola dogru kaymasına engel varmı?
						for(int k=0;k<boy-1;k++)//yoksa
					{
						for(int l=0;l<en-1;l++){
							if(harita[k,l]==4 )
							{
								harita[k,l]=0;//tüm blokları sola dogru kaydır
								harita[k,l-1]=4;
							}
						}
					}break;
				default:
					break;
			}

			
			
		}
		public static void yazdır()
		{
			Console.Clear();
			//haritayı yazdır
			for (int i = 0; i < boy; i++)
			{
				for (int j = 0; j < en; j++)
				{
					switch (harita[i, j])
					{
						case 0:
							Console.Write(" ");
							break;
						case 3:
							Console.Write("+");
							break;
						case 4:
							Console.Write("*");
							break;
						case 5:
							Console.Write("o");
							break;
					}
				}
				Console.WriteLine(" ");
			}
		}
		public static void Main(string[] args)
		{
			/*
			 * Öncelikle arka planda bir timer çalışacak
			 * bu timerin her bir tikinde öncelikle bloklar her saykılda 1 birim aşağıya dogru hareket edecek(eğer en değdiği yer herhangi bir tuğlaya ve sınıra denk gelmdiyse)
			 * bu bloklar ı,t,z,l,kare biçiminde rastgele seçilecektir
			 * 5 tuşuna basıldığında blokların doksan derece çevirilmesi sağlanacaktır
			 * 4 ve 6 tuşları vasıtasıyla yatay olarak hareket ettirilecektir
			 * blok zemine ya da altında bir blok olduğunda ilerlemeyecek oyun tarafından rastgele yeni bir blok gönderilecek,ve yok etme fonksiyonu çalıştırılacaktır
			 * yok etme fonksiyonu ise yatay olarak bir satırda tüm olarak doluysa silinecektir.
			 * */
			Random random = new Random();
			for (int i = 0; i < boy; i++)
			{
				for (int k = 0; k < en; k++)
				{
					
					harita[i, k] = 0;
					if ((((k == 0) || (i == 0)) || (i == boy-1)) || (k == en-1))
					{
						harita[i, k] =3;//oyun başlıyor:)
					}
					rastgele();
					
				}
			}

			t.Elapsed += new ElapsedEventHandler(Program.hareket);
			t.Start();
			while (true)
			{
				
			}
		}
	}
}