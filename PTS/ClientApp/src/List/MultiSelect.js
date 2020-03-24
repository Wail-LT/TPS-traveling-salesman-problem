import React, { Component } from 'react';
import { InputGroup, Input, Button } from 'reactstrap';
import Item from './Item.js';

import './MultiSelect.css'

export default class MultiSelect extends Component {

    constructor(props) {
        super(props);
        this.state = {
            listcity: [],
            searching: false,
            inputVal: ""
        };
        this.onDelete = props.onDelete;
        this.handleClick = this.handleClick.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.render = this.render.bind(this);
    }


    async populateListcity(search) {
        search = search.trim();
        if (search !== "") {
            const response = await fetch('api/city/location?text=' + search);
            const data = await response.json();
            this.setState({
                listcity: data.map(city => { return { name: city.name, zip: city.zip } }),
                searching: true
            });
        }
        else {
            this.setState({ searching: false });
        }

    }

    handleChange(e) {
        var val = e.target.value;
        this.setState({ inputVal: val });
        this.populateListcity(val);
    }

    handleClick(city) {
        this.setState({ inputVal: "" });
        this.props.selectedCities.push(city);
    }


    render() {

        var hiddenList = !this.state.searching && Object.keys(this.props.selectedCities).length == 0;
        var searchContainer = (
            <div className={"ptsContainer" + (this.state.searching ? "" : " validated")}>
                {this.searchResList()}
                {this.selectedList()}
                {this.state.searching && (<div className={"buttonContainer"}>
                    <Button color="primary" onClick={() => {
                        this.setState({ searching: false })
                        this.props.onValidate()}}>Apply changes</Button>
                </div>)}
            </div>);

        return (
            <div>
                <InputGroup>
                    <Input placeholder="Add a city" value={this.state.inputVal} onChange={this.handleChange} />
                </InputGroup>
                {hiddenList || searchContainer}
            </div>
        );
    }


    selectedList = () => {
        return (
            <div className={"cityListContainer"}>
                {this.props.selectedCities.map(city =>
                    <Item key={city.name + "." + city.zip} city={city} onDelete={(city) => { this.onDelete(city) }} />)}
            </div>
        );
    }

    searchResList = () => {
        var content = (<div>
            {this.state.listcity.filter(city => this.props.selectedCities.find(v => city.name + "." + city.zip == v.name + "." + v.zip) === undefined).map(city =>
                <div className="searchRes" key={city.name + "." + city.zip} onClick={() => this.handleClick(city)}>{city.name + " (" + city.zip + ")"}</div>)}
            <hr />
        </div>);
        return this.state.searching ? content : undefined;
    }

}