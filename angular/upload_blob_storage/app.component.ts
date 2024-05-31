import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import {
  BlobServiceClient,
  ContainerClient,
  BlobClient,
  BlockBlobClient,
} from '@azure/storage-blob';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  linkimg: string = '';
  ngOnInit(): void {}
  constructor() {}
  // connect container storage
  connectString: string =
    'BlobEndpoint=https://apinet.blob.core.windows.net/;QueueEndpoint=https://apinet.queue.core.windows.net/;FileEndpoint=https://apinet.file.core.windows.net/;TableEndpoint=https://apinet.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2024-05-16T03:29:15Z&st=2024-05-08T19:29:15Z&spr=https,http&sig=O9ao0ZaM9aJh3hjETblSD7UhejpxIp9ZImNBevwL4c8%3D';
  //

  async uploadImage(file: File): Promise<string> {
    const blobServiceClient = BlobServiceClient.fromConnectionString(
      this.connectString
    );
    const containerClient: ContainerClient =
      blobServiceClient.getContainerClient('images');
    const blobName: string = `${new Date().getTime()}_${file.name}`;
    const blobClient: BlockBlobClient =
      containerClient.getBlockBlobClient(blobName);

    // Upload file to the blob
    blobClient.uploadData(file, {
      blobHTTPHeaders: { blobContentType: file.type },
    });
    // Get the URL of the uploaded blob
    const blobUrl = blobClient.url;
    return blobUrl; //return link images
  }

  async onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      // Handle the selected file
      console.log('Selected file:', file);
      this.linkimg = await this.uploadImage(file);
      let ImagesCut = this.linkimg.split('?');
      console.log(ImagesCut[0]);
    }
  }
}
