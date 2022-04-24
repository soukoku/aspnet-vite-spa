# aspnet-vite-spa
Sample of using vite with aspnet (net6) with aspnet handling the views and vite handling the assets.

## Dev time
Run the typical `npm run dev` in the *vite-app* folder, then debug aspnet as usual.
Aspnet has the controllers and views, and the view specify what script file as entry.
If the vite url changes from https://localhost:3000 then change it accordingly in *Program.cs* file.

## Published
Everything should just work when publishing the aspnet site. Vite makes the assets and 
they becomes static files.
