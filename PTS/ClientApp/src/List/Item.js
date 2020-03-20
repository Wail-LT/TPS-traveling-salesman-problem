import React, { Component } from 'react';
import './Item.css';

export default class Item extends Component {

    constructor(props) {
        super(props);
        this.onDelete = props.onDelete;
        this.city = props.city;
    }

    render(){
        return (
        <div className="itemContainer">
            <span className="label">{this.city.name + " (" + this.city.zip + ")"}</span> 
            <span className="deleteIcon"><img src="plus.svg" onClick={()=>this.onDelete(this.city)}/></span>
        </div>);
    }

}