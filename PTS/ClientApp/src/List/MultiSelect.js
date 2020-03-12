import React, { Component } from 'react';
import {InputGroup,Input} from 'reactstrap';

export default class MultiSelect extends Component {

    constructor(props) {
        super(props);
        this.state = {
            listVille=[],
            selected = []

        }
    }

    inputSearch = (
        <InputGroup>
            <Input placeholder="Saisissez une ville" onClick={this.test}/>
        </InputGroup>);

    test(){
        console.log("ok");
    }
    render() {
        return this.inputSearch;
    }

}