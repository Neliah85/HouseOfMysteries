import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns

file_path = "challenge_results.csv"  
df = pd.read_csv(file_path, encoding='iso-8859-2', sep=';')

if 'roomName' not in df.columns or 'teamName' not in df.columns or 'result' not in df.columns:
    raise ValueError("A szükséges oszlopok ('roomName', 'teamName', 'result') nem találhatók az adatokban.")

df['result_seconds'] = pd.to_timedelta(df['result']).dt.total_seconds()

df = df.dropna(subset=['roomName', 'teamName', 'result_seconds'])

df_sorted = df.sort_values(by=['roomName', 'result_seconds'], ascending=True)
df_top3 = df_sorted.groupby('roomName').head(3)

plt.figure(figsize=(12, 6))
sns.barplot(x='roomName', y='result_seconds', hue='teamName', data=df_top3, palette='viridis')

plt.xlabel('Szoba neve', fontsize=12)
plt.ylabel('Eredmény (másodperc)', fontsize=12)
plt.title('Legjobb 3 csapat eredménye szobánként', fontsize=14)
plt.xticks(rotation=45)
plt.legend(title='Csapatnév', bbox_to_anchor=(1.05, 1), loc='upper left')

plt.tight_layout()
plt.savefig("top3_results.png")  
plt.show()
