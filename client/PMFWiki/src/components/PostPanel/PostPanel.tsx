import { Panel } from 'primereact/panel';
import { Avatar } from 'primereact/avatar';
import './PostPanel.css'
import { Button } from 'primereact/button';
function PostPanel( info : any) {
    
    const headerTemplate = (options : any) => {
        const className = `${options.className} justify-content-space-between`;

        return (
            <div className={className}>
                <div className="d-flex align-items-center gap-2">
                    <Avatar image="https://primefaces.org/cdn/primereact/images/avatar/amyelsner.png" size="large" shape="circle" />
                    <span className="font-bold">Amy Elsner</span>
                </div>
                <div>
                    <Button icon="pi pi-pencil" rounded severity="danger" aria-label="Izmeni" />
                    <Button icon="pi pi-times" rounded severity="danger" aria-label="Obriši" />
                </div>
            </div>
        );
    };

    const footerTemplate = (options : any) => {
        const className = `${options.className} flex flex-wrap align-items-center justify-content-between gap-3`;
        return (
            <div className={className} style={{borderBottomLeftRadius: "0.5rem", borderBottomRightRadius: "0.5rem" }}>
                <span className="p-text-secondary">Updated 2 hours ago</span>
            </div>
        );
    };

    return (
        <Panel header="Header" headerTemplate={headerTemplate} footerTemplate={footerTemplate} toggleable>
            <h1 style={{color: "white"}}>Naslov</h1>
            <p style={{color: "white"}} className="m-0">
                {info.info.name}
            </p>
        </Panel>
    )
}
 
export default PostPanel;