import os
import pdfplumber
from bs4 import BeautifulSoup
import re

def clean_text(text):
    # Gereksiz boşlukları temizle
    text = re.sub(r'\s+', ' ', text)
    # Özel karakterleri temizle
    text = re.sub(r'[^\w\s.,;:?!()\-\'"]', '', text)
    return text.strip()

def convert_pdf_to_html(pdf_path, html_path):
    html_content = ['<!DOCTYPE html>', '<html>', '<head>', 
                   '<meta charset="UTF-8">',
                   '<style>',
                   'body { font-family: Arial, sans-serif; line-height: 1.6; }',
                   'h1, h2, h3 { color: #2c3e50; }',
                   'p { margin-bottom: 1em; }',
                   '</style>',
                   '</head>', '<body>']
    
    with pdfplumber.open(pdf_path) as pdf:
        for page in pdf.pages:
            text = page.extract_text()
            if text:
                # Paragrafları ayır
                paragraphs = text.split('\n\n')
                for para in paragraphs:
                    if para.strip():
                        # Başlık kontrolü
                        if len(para) < 100 and para.isupper():
                            html_content.append(f'<h2>{clean_text(para)}</h2>')
                        else:
                            html_content.append(f'<p>{clean_text(para)}</p>')
    
    html_content.extend(['</body>', '</html>'])
    
    with open(html_path, 'w', encoding='utf-8') as f:
        f.write('\n'.join(html_content))

def main():
    # PDF klasörü
    pdf_dir = 'wwwroot/docs/kanunlar'
    # HTML klasörü
    html_dir = 'wwwroot/docs/html'
    
    # HTML klasörünü oluştur
    os.makedirs(html_dir, exist_ok=True)
    
    # Tüm PDF dosyalarını dönüştür
    for filename in os.listdir(pdf_dir):
        if filename.endswith('.pdf'):
            pdf_path = os.path.join(pdf_dir, filename)
            html_filename = filename.replace('.pdf', '.html')
            html_path = os.path.join(html_dir, html_filename)
            
            print(f'Dönüştürülüyor: {filename}')
            convert_pdf_to_html(pdf_path, html_path)
            print(f'Tamamlandı: {html_filename}')

if __name__ == '__main__':
    main() 