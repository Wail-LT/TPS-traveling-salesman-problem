import React, { Component } from 'react';
import { FormGroup, Input, Label, Row, Col } from 'reactstrap';

import './SelectMethode.css';

export default class SelectMethode extends Component {

    constructor(props) {
        super(props);
        this.methodes = props.methodes;
        this.selectedMethode = props.selectedMethode;
        this.onUpdate = props.onUpdate;

        this.handleUpdate = this.handleUpdate.bind(this);
        this.render = this.render.bind(this);
    }

    handleUpdate(e) {
        if (e.target.id == "selectMethode") {
            this.selectedMethode.name = this.methodes[e.target.value].name;
            this.selectedMethode.factor = this.methodes[e.target.value].factor;
        }
        else
            this.selectedMethode.factor = e.target.value / 100;

        this.onUpdate(this.selectedMethode);
    }

    render() {
        return (
            <FormGroup>
                <Input className="input" id="selectMethode" type="select" name="select" onChange={(e) => this.handleUpdate(e)}>
                    {this.methodes.map((methode, index) =>
                        <option key={index} value={index}>{methode.name}</option>
                    )}
                </Input>

                <div className="slidecontainer">
                    <input type="range" min="1" max="100" value={Math.round(this.selectedMethode.factor * 100)} onChange={(e) => this.handleUpdate(e)} className="slider" id="myRange" />
                </div>
                <div className="slideValue">
                    <span>Mutation Factor : {Math.round(this.selectedMethode.factor * 100)}%</span>
                </div>

            </FormGroup>
        );
    }

}