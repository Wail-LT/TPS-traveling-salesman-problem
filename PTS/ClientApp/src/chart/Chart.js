import React, { Component } from 'react';
import { LineChart, Line, CartesianGrid, Tooltip, XAxis, YAxis } from 'recharts';

export default class Chart extends Component {
    static displayName = Chart.name;

    constructor(props) {
        super(props);
        this.state = {
        };
    }

    render() {
        console.log(this.props.bestRoute)
        return (<div>
            <LineChart width={600} height={300} data={this.props.data}>
                <Line type="monotone" dataKey="fitness" stroke="#8884d8" />
                <CartesianGrid stroke="#ccc" />
                <XAxis dataKey="gen" />
                <YAxis />
                <Tooltip />
            </LineChart>
            <div>
                <p>
                    <span>Method : {this.props.method.name}</span>
                </p>
                <p>
                    <span>Mutation Factor : {this.props.method.factor*100} %</span>
                </p>
                <p><span>Best Route : </span>
                {
                    this.props.bestRoute.map((city, index) => (<span key={index}>{city.name}{ + index == this.props.bestRoute.length -1 || ", "}</span>) )
                }</p>
            </div>
        </div>

    )}

}